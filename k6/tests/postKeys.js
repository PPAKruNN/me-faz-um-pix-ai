// import { sleep } from "k6";
import http from "k6/http";
import { SharedArray } from "k6/data";

const url = "http://localhost:5000";

export const options = {
  vus: 50,
  duration: "30s",
};

console.log("Loading data...");
const data = new SharedArray("users", function () {
  const result = JSON.parse(open("../payloads/accounts.json"));
  return result;
});
console.log("Finished loading...");

export default function () {
  const index = Math.floor(Math.random() * data.length);
  const d = data[index];

  const payload = d.payload;

  const body = JSON.stringify(payload);
  const headers = {
    "Content-Type": "application/json",
    Authorization: "Bearer " + d.token,
  };

  http.post(`${url}/keys`, body, { headers });
}
