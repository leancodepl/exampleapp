import { InfoSelfServiceSettings } from "@leancodepl/kratos";
import { UiNode, UiNodeTextAttributes, UiText } from "@ory/kratos-client";
import { Typography, Tag } from "antd";
import { isObject } from "lodash";
import { FormattedMessage } from "react-intl";
import { Box } from "../../../../components/common/Box";
import { UiMessage } from "../../messages/UiMessage";

type NodeTextProps = {
    node: UiNode;
    attributes: UiNodeTextAttributes;
};

export function NodeText({ attributes, node }: NodeTextProps) {
    return (
        <>
            <Typography.Text>
                <UiMessage attributes={attributes} text={node.meta.label} />
            </Typography.Text>
            <Content attributes={attributes} node={node} />
        </>
    );
}

function Content({ attributes }: NodeTextProps) {
    switch (attributes.text.id) {
        case InfoSelfServiceSettings.InfoSelfServiceSettingsLookupSecretList:
            return (
                <Box gap="medium">
                    {isContextWithSecrets(attributes.text.context) &&
                        attributes.text.context.secrets.map((text: UiText, i: number) => (
                            <Typography.Text key={i} strong>
                                <Tag>
                                    {text.id === InfoSelfServiceSettings.InfoSelfServiceSettingsLookupSecretUsed ? (
                                        <FormattedMessage defaultMessage="Used" />
                                    ) : (
                                        text.text
                                    )}
                                </Tag>
                            </Typography.Text>
                        ))}
                </Box>
            );
    }

    return <Typography.Text strong>{attributes.text.text}</Typography.Text>;
}

type ContextWithSecrets = {
    secrets: UiText[];
};

function isContextWithSecrets(value: unknown): value is ContextWithSecrets {
    return isObject(value) && "secrets" in value && Array.isArray(value.secrets);
}
