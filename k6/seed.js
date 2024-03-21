import { fakerPT_BR as faker } from "@faker-js/faker";
import dotenv from "dotenv";
import knex from "knex";
import fs from "fs";

dotenv.config();

const db = knex({
  client: "pg",
  connection: process.env.DATABASE_URL,
});

// Records number.

const same = 1000000;

const USERS = same;
const PAYMENTPROVIDER = same;
const ACCOUNTS = same;
const PIXKEYS = same;

const CLEARDB = true;

// Tables name:
const USER_TABLE = "User";
const PAYMENTPROVIDER_TABLE = "PaymentProvider";
const ACCOUNTS_TABLE = "PaymentProviderAccount";
const PIXKEYS_TABLE = "PixKey";

run();

async function run() {
  if (CLEARDB) {
    console.log("Deleting database");
    console.log("Deleting Usertable");
    await db(USER_TABLE).del();
    console.log("Deleting PaymentProvider");
    await db(PAYMENTPROVIDER_TABLE).del();
    console.log("Deleting AccountsTable");
    await db(ACCOUNTS_TABLE).del();
    console.log("Deleting PixKeysTable");
    await db(PIXKEYS_TABLE).del();
  }

  const start = new Date();

  const users = generateUsersData();
  const usersIds = await save(USER_TABLE, users);

  const psps = generatePaymentProviderData();
  const pspIds = await save(PAYMENTPROVIDER_TABLE, psps);

  const accounts = generateAccounts(usersIds, pspIds);
  const accountsIds = await save(ACCOUNTS_TABLE, accounts);

  saveToFile("./payloads/accounts.json", users, psps, accounts);

  const keys = generateKeys(accountsIds);
  const keysIds = await save(PIXKEYS_TABLE, keys);

  const formatedKeys = keys.map((key, index) => {
    return { type: key.Type, value: key.Value, token: psps[index].Token };
  });
  generateJson("./payloads/keys.json", formatedKeys);

  const paymentsData = accounts.map((a, i) => {
    return {
      Token: psps[i].Token,
      Agency: a.Agency,
      Number: a.Number,
      Cpf: users[i].CPF,
    };
  });

  const paymentsDB = [];
  accounts.forEach((a, i) => {
    if (i === 0) return;

    paymentsDB.push({
      Status: faker.helpers.arrayElement([
        "PROCESSING",
        "ACCEPTED",
        "REJECTED",
      ]),
      Amount: faker.number.int({ min: 1, max: 10000 }),
      Description: faker.helpers.arrayElement([faker.lorem.sentence(), null]),
      PixKeyId: keysIds[i].Id,
      OriginPaymentProviderAccountId: accountsIds[0].Id,
      DestinationPaymentProviderAccountId: accountsIds[i].Id,
    });
  });

  generateJson("./payloads/payments.json", paymentsData);
  await save("Payment", paymentsDB);

  const end = new Date();

  db.destroy();

  console.log("Done!");
  console.log("Finished in %d seconds", (end - start) / 1000);
}

function saveToFile(filepath, users, psps, accounts) {
  console.log("Mapping and saving to file");
  const data = [];

  for (let i = 0; i < accounts.length; i++) {
    data.push({
      token: psps[i].Token,
      payload: {
        key: generateKeyPayload(),
        user: {
          cpf: users[i].CPF,
        },
        account: {
          number: accounts[i].Number,
          agency: accounts[i].Agency,
        },
      },
    });
  }

  generateJson(filepath, data);
}

function generateKeyPayload() {
  const type = faker.helpers.arrayElement(["Random", "CPF", "Email", "Phone"]);
  let value = "";

  switch (type) {
    case "Random":
      value = faker.string.uuid();
      break;

    case "CPF":
      value = faker.string.numeric({ length: 11 });
      break;

    case "Email":
      value = faker.internet.email();
      break;

    case "Phone":
      value = faker.string.numeric({ length: 13 });
      break;
  }

  return {
    type,
    value,
  };
}

function generateUsersData() {
  console.log("User seed");
  const users = [];

  for (let i = 0; i < USERS; i++) {
    users.push({
      CPF: faker.string.numeric({ length: 11 }),
      Name: faker.person.fullName(),
      CreatedAt: new Date(),
      UpdatedAt: new Date(),
    });
  }
  return users;
}

function generatePaymentProviderData() {
  console.log("PSP/BANK/PaymentProvider seed");
  const psps = [];

  for (let i = 0; i < PAYMENTPROVIDER; i++) {
    psps.push({
      Name: faker.company.name(),
      Token: faker.string.uuid(),
      ProcessingWebhook: "http://localhost:5039/payments/pix",
      AcknowledgeWebhook: "http://localhost:5039/payments/pix",
      CreatedAt: new Date(),
      UpdatedAt: new Date(),
    });
  }
  return psps;
}

function generateAccounts(userIds, pspIds) {
  console.log("Accounts seed");
  const accounts = [];

  for (let i = 0; i < ACCOUNTS; i++) {
    accounts.push({
      Agency: faker.number.int({ min: 1, max: 1000 }).toString(),
      Number: faker.number.int({ min: 1, max: 0x7fffffff }).toString(),
      UserId: userIds[i].Id.toString(),
      PaymentProviderId: pspIds[i].Id.toString(),
      CreatedAt: new Date(),
      UpdatedAt: new Date(),
    });
  }
  return accounts;
}

function generateKeys(accountsIds) {
  console.log("Keys seed");
  const keys = [];

  for (let i = 0; i < PIXKEYS; i++) {
    const type = faker.helpers.arrayElement([
      "CPF",
      "Email",
      "Phone",
      "Random",
    ]);
    let value;

    switch (type) {
      case "CPF":
        value = faker.string.numeric({ length: 11 });
        break;

      case "Email":
        value = faker.internet.email();
        break;

      case "Phone":
        value = faker.string.numeric({ length: 13 });
        break;

      case "Random":
        value = faker.string.uuid();
        break;
    }

    keys.push({
      Type: type,
      Value: value,
      PaymentProviderAccountId: accountsIds[i].Id.toString(),
      CreatedAt: new Date(),
      UpdatedAt: new Date(),
    });
  }
  return keys;
}

async function save(table, data) {
  console.log("Saving: ", table);
  const result = await db.batchInsert(table, data).returning("Id");
  return result;
}

function generateJson(filepath, data) {
  if (fs.existsSync(filepath)) {
    fs.unlinkSync(filepath);
  }

  console.log("Generating JSON file");
  console.log("Saving to: ", filepath);

  fs.writeFileSync(filepath, JSON.stringify(data));
}
