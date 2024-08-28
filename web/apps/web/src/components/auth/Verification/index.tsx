import { FormattedMessage } from "react-intl"
import { Link, useNavigate } from "react-router-dom"
import { Divider, Spin } from "antd"
import { VerificationCard, useVerificationFlow } from "@leancodepl/kratos"
import { kratosClient } from "../../../auth"
import { loginRoute } from "../../../kratosRoutes"
import { Box } from "../../common/Box"
import { CardTitle } from "../_common/MarginlessTitle"

export function Verification() {
    const nav = useNavigate()
    const { flow, submit } = useVerificationFlow({
        kratosClient,
        onVerified: () => {
            nav(loginRoute)
        },
    })

    return (
        <Box direction="column" gap="small">
            <CardTitle>
                <FormattedMessage defaultMessage="Verification" />
            </CardTitle>
            {flow ? <VerificationCard flow={flow} onSubmit={submit} /> : <Spin size="large" />}
            <Divider />
            <Box gap="small" justify="center">
                <FormattedMessage defaultMessage="Don't have an account?" />
                <Link to={loginRoute}>
                    <FormattedMessage defaultMessage="Sign up" />
                </Link>
            </Box>
        </Box>
    )
}
