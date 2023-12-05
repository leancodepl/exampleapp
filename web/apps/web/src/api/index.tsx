import { createApiComponents } from "@leancodepl/admin";
import { TokenProvider } from "@leancodepl/cqrs-client-base";
import { mkCqrsClient } from "@leancodepl/react-query-cqrs-client";
import { QueryClient } from "@tanstack/react-query";
import schema from "./api-components-schema";
import cqrs from "./cqrs";
import { sessionManager } from "../auth/sessionManager";
import { environment } from "../environments/environment";

export const tokenProvider: Partial<TokenProvider> = {
    invalidateToken: async () => {
        sessionManager.checkIfLoggedIn();
        return true;
    },
};

export const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: 3,
        },
    },
});

const cqrsClientConfig = {
    cqrsEndpoint: `${environment.apiUrl}/api`,
    queryClient,
    tokenProvider,
};

const mkApiClient = { ...mkCqrsClient(cqrsClientConfig), createTopic: () => void 0 } as ReturnType<
    typeof mkCqrsClient
> & {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    createTopic: <TTopic, TNotifications extends Record<string, unknown>>(endpoint: string) => unknown;
};

export type ReactQueryCqrs = typeof mkApiClient;

const rawApi = cqrs(mkApiClient);

type PrefixWith<T, TPrefix extends string> = {
    [K in keyof T as K extends string ? `${TPrefix}${K}` : never]: T[K];
};

function addPrefix<T extends object, TPrefix extends string>(object: T, prefix: TPrefix) {
    return Object.fromEntries(Object.entries(object).map(([key, value]) => [`${prefix}${key}`, value])) as PrefixWith<
        T,
        TPrefix
    >;
}

export const api = addPrefix(rawApi, "use");
export const apiComponents = createApiComponents(schema, { cqrsClientConfig, cqrs });
