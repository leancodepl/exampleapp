import { useCallback } from "react";
import { ResponseError, aalParameterName, returnToParameterName } from "@leancodepl/kratos";
import { Typography, Spin } from "antd";
import { useNavigate } from "react-router";
import { useSearchParams } from "react-router-dom";
import { Flow, useSignInFlow } from "../../../auth";
import { kratosClient } from "../../../auth/ory";
import { sessionManager } from "../../../auth/sessionManager";
import { signInRoute } from "../../../routes";
import { Box } from "../../common/Box";
import { NodesWrapper } from "../NodesWrapper";

export function SignIn() {
    const handleSignIn = useHandleSignIn();

    const { flow, submit } = useSignInFlow({
        kratosClient,
        signInRoute,
        onSignedIn: handleSignIn,
        onSessionAlreadyAvailable: useCallback(() => {
            sessionManager.checkIfSignedIn();
        }, []),
    });

    return (
        <Box direction="column" gap="large">
            <Typography.Title level={4}>Sign In</Typography.Title>

            {flow ? (
                <Flow flow={flow} nodesWrapper={NodesWrapper} onSubmit={submit} />
            ) : (
                <Box align="center" justify="center">
                    <Spin size="large" />
                </Box>
            )}
        </Box>
    );
}

function useHandleSignIn() {
    const nav = useNavigate();
    const [search] = useSearchParams();
    const returnTo = search.get(returnToParameterName);

    return useCallback(async () => {
        try {
            const session = (await kratosClient.toSession()).data;
            sessionManager.setSession(session);

            if (returnTo) {
                nav(returnTo);
                return;
            }
        } catch (err) {
            const data = (err as ResponseError).response?.data;
            switch (data.error.code) {
                case 403:
                    if (data.error?.id === "session_aal2_required") {
                        nav(`${signInRoute}?${aalParameterName}=aal2`, { replace: true });
                    }
                    break;
            }
        }
    }, [nav, returnTo]);
}
