import { useCallback } from "react";
import { LoginCard, returnToParameterName, useLoginFlow, useLogoutFlow } from "@leancodepl/kratos";
import { AuthenticatorAssuranceLevel } from "@ory/client";
import { Spin, Divider, Button } from "antd";
import { FormattedMessage } from "react-intl";
import { useNavigate } from "react-router";
import { Link, useSearchParams } from "react-router-dom";
import { kratosClient } from "../../../auth";
import { sessionManager } from "../../../auth/sessionManager";
import { loginRoute, recoveryRoute, registerRoute } from "../../../publicRoutes";
import { Box } from "../../common/Box";
import { CardTitle } from "../_common/MarginlessTitle";

export function Login() {
    const handleLogin = useHandleLogin();
    const nav = useNavigate();
    const { logout } = useLogoutFlow({ kratosClient, onLoggedOut: () => nav(loginRoute) });
    const { flow, submit } = useLoginFlow({
        kratosClient,
        loginRoute,
        onLoggedIn: handleLogin,
        onSessionAlreadyAvailable: useCallback(() => nav("/"), [nav]),
    });

    const isRefresh = flow?.refresh;
    const is2FA = flow?.requested_aal === AuthenticatorAssuranceLevel.Aal2;

    return (
        <Box direction="column" gap="large">
            <CardTitle>
                {is2FA ? (
                    <FormattedMessage defaultMessage="Two-factor authentication" />
                ) : (
                    <FormattedMessage defaultMessage="Sign In" />
                )}
            </CardTitle>
            {flow ? (
                <Box direction="column">
                    <LoginCard flow={flow} onSubmit={submit} />
                    {!is2FA && !isRefresh && (
                        <Box justify="center" padding={{ top: "medium" }}>
                            <Link to={recoveryRoute}>
                                <FormattedMessage defaultMessage="Forgot password?" />
                            </Link>
                        </Box>
                    )}
                    {!isRefresh && (
                        <>
                            <Divider />
                            {is2FA ? (
                                <Button onClick={logout}>
                                    <FormattedMessage defaultMessage="Sign out" />
                                </Button>
                            ) : (
                                <Box gap="small" justify="center">
                                    <FormattedMessage defaultMessage="Don't have an account?" />
                                    <Link to={registerRoute}>
                                        <FormattedMessage defaultMessage="Sign up" />
                                    </Link>
                                </Box>
                            )}
                        </>
                    )}
                </Box>
            ) : (
                <Box align="center" justify="center">
                    <Spin size="large" />
                </Box>
            )}
        </Box>
    );
}

function useHandleLogin() {
    const nav = useNavigate();
    const [search] = useSearchParams();
    const returnTo = search.get(returnToParameterName);

    return useCallback(async () => {
        const session = (await kratosClient.toSession()).data;
        sessionManager.setSession(session);

        if (returnTo) {
            nav(returnTo);
            return;
        }
    }, [nav, returnTo]);
}
