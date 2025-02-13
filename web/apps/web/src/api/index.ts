import { QueryClient } from "@tanstack/react-query"
import { mkCqrsClient } from "@leancodepl/react-query-cqrs-client"
import { addPrefix } from "@leancodepl/utils"
import { sessionManager } from "../auth/sessionManager"
import { environment } from "../environments/environment"
import cqrs from "./cqrs"

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 10 * 1000,
      retry: 3,
    },
  },
})

const mkApiClient = {
  ...mkCqrsClient({
    cqrsEndpoint: `${environment.apiUrl}/api`,
    queryClient,
    tokenProvider: {
      invalidateToken: () => {
        sessionManager.checkIfLoggedIn()
        return Promise.resolve(true)
      },
    },
  }),
  // eslint-disable-next-line @typescript-eslint/no-unused-vars, unused-imports/no-unused-vars
  createTopic<T1, T2>(name: string) {},
}

const rawApi = cqrs(mkApiClient)

export type ReactQueryCqrs = typeof mkApiClient

export type Api = typeof api
export const api = addPrefix(rawApi, "use")
