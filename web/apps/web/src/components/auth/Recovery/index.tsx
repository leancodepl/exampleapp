import { FormattedMessage } from "react-intl"
import { Link } from "react-router-dom"
import { Divider, Spin } from "antd"
import { RecoveryCard, flowIdParameterName, useRecoveryFlow } from "@leancodepl/kratos"
import { kratosClient } from "../../../auth"
import { sessionManager } from "../../../auth/sessionManager"
import { loginRoute, recoveryRoute, settingsRoute } from "../../../kratosRoutes"
import { Box } from "../../common/Box"
import { CardTitle } from "../_common/MarginlessTitle"

export function Recovery() {
    const { flow, submit } = useRecoveryFlow({
        kratosClient,
        recoveryRoute,
        onSessionAlreadyAvailable: () => {
            sessionManager.checkIfLoggedIn()
        },
        onContinueWith: continueWith => {
            if (continueWith[0]?.action === "show_settings_ui") {
                const url = new URL(settingsRoute, window.location.origin)
                url.searchParams.set(flowIdParameterName, continueWith[0].flow.id)
                window.location.href = url.toString()
            }
        },
    })

    return (
        <Box $direction="column" $gap="small">
            <CardTitle>
                <FormattedMessage defaultMessage="Recovery" />
            </CardTitle>
            {flow ? <RecoveryCard flow={flow} onSubmit={submit} /> : <Spin size="large" />}
            <Divider />
            <Box $gap="small" $justify="center">
                <Link to={loginRoute}>
                    <FormattedMessage defaultMessage="Go back to sign in page" />
                </Link>
            </Box>
        </Box>
    )
}
