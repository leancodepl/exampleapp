const preamble = `
/* eslint-disable no-redeclare, import/no-anonymous-default-export, unused-imports/no-unused-vars, @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-interface, @typescript-eslint/no-namespace */
import { ApiDateOnly, ApiDateTimeOffset, ApiTimeSpan } from "@leancodepl/api-date-datefns"
import type { ReactQueryCqrs as CQRS } from ".";

export type Query<TResult> = {}
export type Command = {}
export type Operation<TResult> = {}
export type Topic = {}

`.trimStart()

/**
 * @type {import("@leancodepl/contractsgenerator-typescript-plugin-contracts/src/configuration").ContractsGeneratorPluginConfiguration}
 */
const config = {
  customTypes: {
    DateOnly: "ApiDateOnly",
    DateTimeOffset: "ApiDateTimeOffset",
    TimeSpan: "ApiTimeSpan",
  },
  input: {
    base: "../../../backend/src",
    project: ["Examples/ExampleApp.Examples.Contracts/ExampleApp.Examples.Contracts.csproj"],
  },
  nameTransform: nameWithNamespace => {
    const nameParts = nameWithNamespace.split(".")
    let name = nameParts.at(-1)

    if (nameParts.at(1) === "ForceUpdate") name = `ForceUpdate${name}`

    return name
  },
}

/**
 * @type {import("@leancodepl/contractsgenerator-typescript").ContractsGeneratorConfiguration}
 */
module.exports = {
  generates: {
    "src/api/cqrs.ts": {
      plugins: [{ raw: { prepend: preamble } }, "contracts", "client"],
    },
  },
  config,
}
