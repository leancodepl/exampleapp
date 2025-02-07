const preamble = `
/*eslint-disable import/no-anonymous-default-export, unused-imports/no-unused-vars, @typescript-eslint/no-unused-vars, @typescript-eslint/ban-types, @typescript-eslint/no-empty-interface, @typescript-eslint/no-namespace, @nx/enforce-module-boundaries, @typescript-eslint/no-explicit-any*/
import type { ApiDateTimeOffset } from "@leancodepl/api-date-datefns"
import type { ReactQueryCqrs as CQRS } from ".";

export type Query<TResult> = {}
export type Command = {}
export type Operation<TResult> = {}
export type Topic = {}

`.trimStart()

module.exports = {
    generates: {
        "src/api/api-components-schema.ts": {
            plugins: ["admin"],
        },
        "src/api/cqrs.ts": {
            plugins: [{ raw: { prepend: preamble } }, "contracts", "client"],
        },
    },
    config: {
        customTypes: {
            DateTimeOffset: "ApiDateTimeOffset",
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
    },
}
