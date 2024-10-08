const { withReact } = require("@nx/react")
const { composePlugins, withNx } = require("@nx/webpack")

module.exports = composePlugins(withNx(), withReact(), config => {
    config.devServer = {
        ...config.devServer,
        host: "0.0.0.0",
        client: {
            ...config.devServer?.client,
            webSocketURL: {
                hostname: "local.lncd.pl",
                port: 443,
                pathname: "/ws",
                protocol: "wss",
            },
        },
    }

    return config
})
