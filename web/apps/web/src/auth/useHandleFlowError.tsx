import { useCallback } from "react";
import { ErrorId, ResponseError } from "@leancodepl/kratos";
import { App } from "antd";
import { FormattedMessage } from "react-intl";

export type FlowErrorResponse = {
    error?: {
        id?: ErrorId;
    };
    redirect_browser_to: string;
    use_flow_id?: string;
};

export function useHandleFlowError({
    resetFlow,
    onSessionAlreadyAvailable,
}: {
    resetFlow: (newFlowId?: string) => void;
    onSessionAlreadyAvailable?: () => void;
}) {
    const { message } = App.useApp();

    return useCallback(
        async (err: ResponseError<FlowErrorResponse>) => {
            switch (err.response?.data.error?.id) {
                case ErrorId.ErrIDHigherAALRequired:
                    // 2FA is enabled and enforced, but user did not perform 2fa yet!
                    window.location.href = err.response.data.redirect_browser_to;
                    return;
                case ErrorId.ErrIDAlreadyLoggedIn:
                    onSessionAlreadyAvailable?.();
                    return;
                case ErrorId.ErrIDNeedsPrivilegedSession:
                    // We need to re-authenticate to perform this action
                    window.location.href = err.response.data.redirect_browser_to;
                    return;
                case ErrorId.ErrIDRedirectURLNotAllowed:
                    message.error(<FormattedMessage defaultMessage="The return_to address is not allowed." />);
                    resetFlow();
                    return;
                case ErrorId.ErrIDSelfServiceFlowExpired:
                    // The flow expired, let's request a new one.
                    message.error(
                        <FormattedMessage defaultMessage="Your interaction expired, please fill out the form again." />,
                    );
                    resetFlow();
                    return;
                case ErrorId.ErrIDSelfServiceFlowReplaced:
                    resetFlow(err.response.data.use_flow_id);
                    return;
                case ErrorId.ErrIDCSRF:
                    message.error(
                        <FormattedMessage defaultMessage="A security violation was detected, please fill out the form again." />,
                    );
                    resetFlow();
                    return;
                case ErrorId.ErrIDInitiatedBySomeoneElse:
                    // The requested item was intended for someone else. Let's request a new flow...
                    resetFlow();
                    return;
                case ErrorId.ErrIDSelfServiceBrowserLocationChangeRequiredError:
                    // Ory Kratos asked us to point the user to this URL.
                    window.location.href = err.response.data.redirect_browser_to;
                    return;
            }

            switch (err.response?.status) {
                case 401:
                case 410:
                    resetFlow();
                    return;
            }

            return Promise.reject(err);
        },
        [message, onSessionAlreadyAvailable, resetFlow],
    );
}
