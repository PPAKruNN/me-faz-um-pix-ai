// import { sleep } from "k6";
import http from "k6/http";
import { SharedArray } from "k6/data";

const url = "http://localhost:5000";

export const options = {
  vus: 50,
  duration: "30s",
};

console.log("Loading data...");
const payments = new SharedArray("payments", function () {
  const result = JSON.parse(open("../payloads/payments.json"));
  return result;
});

const keys = new SharedArray("keys", function () {
  const result = JSON.parse(open("../payloads/keys.json"));
  return result;
});
console.log("Finished loading...");

export default function () {
  const index = Math.floor(Math.random() * payments.length);
  const payment = payments[index];
  const key = keys[index + 1];

  const body = {
    origin: {
      user: {
        cpf: payment.Cpf,
      },
      account: {
        number: payment.Number,
        agency: payment.Agency,
      },
    },
    destiny: {
      key: {
        type: key.type,
        value: key.value,
      },
    },
    amount: Math.floor(Math.random() * 10000),
    description: "Test payment",
  };

  const payload = JSON.stringify(body);

  const headers = {
    "Content-Type": "application/json",
    Authorization: "Bearer " + payment.Token,
  };

  http.post(`${url}/Payments`, payload, { headers });
}
