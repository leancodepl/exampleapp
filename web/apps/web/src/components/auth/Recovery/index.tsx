import { RecoveryCard, useRecoveryFlow } from "@leancodepl/kratos";
import { Spin, Divider } from "antd";
import { FormattedMessage } from "react-intl";
import { Link } from "react-router-dom";
import { kratosClient } from "../../../auth";
import { sessionManager } from "../../../auth/sessionManager";
import { loginRoute, recoveryRoute } from "../../../publicRoutes";
import { Box } from "../../common/Box";
import { CardTitle } from "../_common/MarginlessTitle";

export function Recovery() {
    const { flow, submit } = useRecoveryFlow({
        kratosClient,
        recoveryRoute,
        onSessionAlreadyAvailable: () => {
            sessionManager.checkIfLoggedIn();
        },
    });

    return (
        <Box direction="column" gap="small">
            <CardTitle>
                <FormattedMessage defaultMessage="Recovery" />
            </CardTitle>
            {flow ? <RecoveryCard flow={flow} onSubmit={submit} /> : <Spin size="large" />}
            <Divider />
            <Box gap="small" justify="center">
                <Link to={loginRoute}>
                    <FormattedMessage defaultMessage="Go back to sign in page" />
                </Link>
            </Box>
        </Box>
    );
}
