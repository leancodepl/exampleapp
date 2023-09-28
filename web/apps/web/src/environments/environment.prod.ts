import { Environment } from "./environment";

declare let app_config: {
    readonly NX_API_BASE: string;
    readonly NX_AUTH_BASE: string;
};

export const environment: Environment = {
    apiUrl: app_config.NX_API_BASE,
    authUrl: app_config.NX_AUTH_BASE,
};
