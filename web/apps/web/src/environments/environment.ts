declare global {
  // eslint-disable-next-line no-var
  var app_config:
    | {
        readonly NX_API_BASE: string
        readonly NX_AUTH_BASE: string
      }
    | undefined
}

export const environment = {
  apiUrl: globalThis.app_config?.NX_API_BASE ?? import.meta.env.VITE_API_BASE,
  authUrl: globalThis.app_config?.NX_AUTH_BASE ?? import.meta.env.VITE_AUTH_BASE,
}
