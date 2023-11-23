const preamble = `
/*eslint-disable import/no-anonymous-default-export, unused-imports/no-unused-vars-ts, @typescript-eslint/no-unused-vars, @typescript-eslint/ban-types, @typescript-eslint/no-empty-interface, @typescript-eslint/no-namespace, @nrwl/nx/enforce-module-boundaries*/
import type { ApiDateTimeOffset } from "@leancodepl/api-date-datefns"
import type { ReactQueryCqrs as CQRS } from ".";

export type Query<TResult> = {}
export type Command = {}
export type Operation<TResult> = {}

`.trimStart();

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
            project: ["Core/ExampleApp.Core.Contracts/ExampleApp.Core.Contracts.csproj"],
        },
        nameTransform: nameWithNamespace => nameWithNamespace.split(".").at(-1),
    },
};
