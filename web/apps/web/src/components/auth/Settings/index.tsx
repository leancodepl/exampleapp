import { KratosContextProvider, UserSettingsCard, useSettingsFlow } from "@leancodepl/kratos";
import { Typography, Spin, Card } from "antd";
import { FormattedMessage } from "react-intl";
import { kratosClient } from "../../../auth";
import { UiMessages } from "../../../auth/ui/components/UiMessages";
import { path } from "../../../routes";
import { Box } from "../../common/Box";

export function Settings() {
    const { flow, submit } = useSettingsFlow({
        kratosClient,
        settingsRoute: path("settings"),
    });

    return (
        <Box direction="column" gap="large">
            <Card>
                <Typography.Title level={2} style={{ margin: 0 }}>
                    <FormattedMessage defaultMessage="Settings" />
                </Typography.Title>
                <UiMessages uiMessages={flow?.ui?.messages} />

                <KratosContextProvider components={{ UiMessages: () => null }}>
                    {flow ? (
                        <>
                            <div>
                                <Typography.Title level={4}>
                                    <FormattedMessage defaultMessage="Lookup secret" />
                                </Typography.Title>
                                <UserSettingsCard flow={flow} flowType="lookupSecret" onSubmit={submit} />
                            </div>
                            <div>
                                <Typography.Title level={4}>
                                    <FormattedMessage defaultMessage="Oidc" />
                                </Typography.Title>
                                <UserSettingsCard flow={flow} flowType="oidc" onSubmit={submit} />
                            </div>
                            <div>
                                <Typography.Title level={4}>
                                    <FormattedMessage defaultMessage="Password" />
                                </Typography.Title>
                                <UserSettingsCard flow={flow} flowType="password" onSubmit={submit} />
                            </div>
                            <div>
                                <Typography.Title level={4}>
                                    <FormattedMessage defaultMessage="Profile" />
                                </Typography.Title>
                                <UserSettingsCard flow={flow} flowType="profile" onSubmit={submit} />
                            </div>
                            <div>
                                <Typography.Title level={4}>
                                    <FormattedMessage defaultMessage="Totp" />
                                </Typography.Title>
                                <UserSettingsCard flow={flow} flowType="totp" onSubmit={submit} />
                            </div>
                            <div>
                                <Typography.Title level={4}>
                                    <FormattedMessage defaultMessage="Webauthn" />
                                </Typography.Title>
                                <UserSettingsCard flow={flow} flowType="webauthn" onSubmit={submit} />
                            </div>
                        </>
                    ) : (
                        <Box align="center" justify="center">
                            <Spin size="large" />
                        </Box>
                    )}
                </KratosContextProvider>
            </Card>
        </Box>
    );
}
