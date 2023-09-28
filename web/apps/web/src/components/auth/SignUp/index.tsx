import { useCallback } from "react";
import { Typography, Spin, Space } from "antd";
import { Flow, useSignUpFlow } from "../../../auth";
import { kratosClient } from "../../../auth/ory";
import { sessionManager } from "../../../auth/sessionManager";
import { signUpRoute } from "../../../routes";
import { Box } from "../../common/Box";
import { NodesWrapper } from "../NodesWrapper";

export function SignUp() {
    const { flow, submit, isSignedUp } = useSignUpFlow({
        kratosClient,
        signUpRoute,
        onSessionAlreadyAvailable: useCallback(() => {
            sessionManager.checkIfSignedIn();
        }, []),
    });

    if (isSignedUp) {
        return (
            <Box direction="column">
                <Space direction="vertical" size="middle">
                    <Typography.Text strong>Sprawdź swoją skrzynkę</Typography.Text>
                    <Typography.Text>
                        Na podany adres e-mail wysłaliśmy Ci wiadomość z linkiem do wpisania kodu aktywacyjnego. Kliknij
                        w link i wpisz kod, aby potwierdzić adres e-mail i aktywować konto.
                    </Typography.Text>
                </Space>
            </Box>
        );
    }

    return (
        <Box direction="column" gap="large">
            <Space direction="vertical" size="middle">
                {flow ? <Flow flow={flow} nodesWrapper={NodesWrapper} onSubmit={submit} /> : <Spin size="large" />}
            </Space>
        </Box>
    );
}
