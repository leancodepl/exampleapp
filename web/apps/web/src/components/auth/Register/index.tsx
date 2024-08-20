import { useCallback } from "react"
import { FormattedMessage } from "react-intl"
import { Link } from "react-router-dom"
import { Divider, Space, Spin, Typography } from "antd"
import { RegistrationCard, flowIdParameterName, useRegisterFlow } from "@leancodepl/kratos"
import { kratosClient } from "../../../auth"
import { sessionManager } from "../../../auth/sessionManager"
import { loginRoute, registerRoute, verificationRoute } from "../../../kratosRoutes"
import { Box } from "../../common/Box"
import { CardTitle } from "../_common/MarginlessTitle"

export function Register() {
    const { flow, submit, isRegistered } = useRegisterFlow({
        kratosClient,
        registrationRoute: registerRoute,
        onSessionAlreadyAvailable: useCallback(() => {
            sessionManager.checkIfLoggedIn()
        }, []),
        onContinueWith: continueWith => {
            if (continueWith[0]?.action === "show_verification_ui") {
                const url = new URL(verificationRoute, window.location.origin)
                url.searchParams.set(flowIdParameterName, continueWith[0].flow.id)
                window.location.href = url.toString()
            }
        },
    })

    if (isRegistered) {
        return (
            <Box direction="column">
                <Space direction="vertical" size="middle">
                    <Typography.Text strong>
                        <FormattedMessage defaultMessage="E-mail sent. Check your mailbox" />
                    </Typography.Text>
                </Space>
            </Box>
        )
    }

    return (
        <Box direction="column">
            <CardTitle>
                <FormattedMessage defaultMessage="Sign up" />
            </CardTitle>
            {flow ? <RegistrationCard flow={flow} onSubmit={submit} /> : <Spin size="large" />}
            <Divider />
            <Box gap="small" justify="center">
                <FormattedMessage defaultMessage="Already have an account?" />
                <Link to={loginRoute}>
                    <FormattedMessage defaultMessage="Sign in" />
                </Link>
            </Box>
        </Box>
    )
}
