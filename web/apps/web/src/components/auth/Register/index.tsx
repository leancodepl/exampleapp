import { useCallback } from "react";
import { RegistrationCard, useRegisterFlow } from "@leancodepl/kratos";
import { Typography, Spin, Space, Divider } from "antd";
import { FormattedMessage } from "react-intl";
import { Link } from "react-router-dom";
import { kratosClient } from "../../../auth";
import { sessionManager } from "../../../auth/sessionManager";
import { loginRoute, registerRoute } from "../../../kratosRoutes";
import { Box } from "../../common/Box";
import { CardTitle } from "../_common/MarginlessTitle";

export function Register() {
    const { flow, submit, isRegistered } = useRegisterFlow({
        kratosClient,
        registrationRoute: registerRoute,
        onSessionAlreadyAvailable: useCallback(() => {
            sessionManager.checkIfLoggedIn();
        }, []),
    });

    if (isRegistered) {
        return (
            <Box direction="column">
                <Space direction="vertical" size="middle">
                    <Typography.Text strong>
                        <FormattedMessage defaultMessage="E-mail sent. Check your mailbox" />
                    </Typography.Text>
                </Space>
            </Box>
        );
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
    );
}
