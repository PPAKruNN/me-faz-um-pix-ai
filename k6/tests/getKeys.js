import http from "k6/http";
import { SharedArray } from "k6/data";

const url = "http://localhost:5000";

export const options = {
  vus: 10,
  duration: "10s",
};

console.log("Loading keys...");
const data = new SharedArray("keys", function () {
  const result = JSON.parse(open("../payloads/keys.json"));
  return result;
});
console.log("Finished Loading!");

// { type: key.Type, value: key.Value, token: psps[index].Token };

export default function () {
  const randomIndex = Math.floor(Math.random() * data.length);

  const currData = data[randomIndex];

  const headers = {
    "Content-Type": "application/json",
    Authorization: `Bearer ${currData.token}`,
  };

  http.get(`${url}/keys/${currData.type}/${currData.value}`, {
    headers,
  });
}
