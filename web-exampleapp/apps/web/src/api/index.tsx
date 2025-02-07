import { QueryClient } from "@tanstack/react-query"
import { createApiComponents } from "@leancodepl/admin"
import { TokenProvider } from "@leancodepl/cqrs-client-base"
import { mkPipeClient } from "@leancodepl/hook-pipe-client"
import { Pipe } from "@leancodepl/pipe"
import { mkCqrsClient } from "@leancodepl/react-query-cqrs-client"
import { addPrefix } from "@leancodepl/utils"
import { sessionManager } from "../auth/sessionManager"
import { environment } from "../environments/environment"
import schema from "./api-components-schema"
import cqrs from "./cqrs"

export const tokenProvider: Partial<TokenProvider> = {
    invalidateToken: async () => {
        sessionManager.checkIfLoggedIn()
        return true
    },
}

export const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: 3,
        },
    },
})

const cqrsClientConfig = {
    cqrsEndpoint: `${environment.apiUrl}/api`,
    queryClient,
    tokenProvider,
}

const pipe = new Pipe({
    url: `${environment.apiUrl}/leanpipe`,
})

const mkApiClient = { ...mkCqrsClient(cqrsClientConfig), ...mkPipeClient({ pipe }) }

export type ReactQueryCqrs = typeof mkApiClient

const rawApi = cqrs(mkApiClient)

export const api = addPrefix(rawApi, "use")
export const apiComponents = createApiComponents(schema, { cqrsClientConfig, cqrs })
