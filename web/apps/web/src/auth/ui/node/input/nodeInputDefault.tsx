import { UiNode, UiNodeInputAttributes } from "@ory/kratos-client";
import { Form, Input } from "antd";
import { useController, useFormContext } from "react-hook-form";
import { getMessages } from "../../messages/getMessages";
import { UiMessage, useCustomUiMessageContext } from "../../messages/UiMessage";

type NodeInputDefaultProps = {
    node: UiNode;
    attributes: UiNodeInputAttributes;
    disabled: boolean;
};

export function NodeInputDefault({ node, disabled, attributes }: NodeInputDefaultProps) {
    const customUiMessage = useCustomUiMessageContext();
    const { control } = useFormContext();

    const { error, info } = getMessages({
        messages: node.messages,
        attributes,
        customUiMessage,
        maxMessagesPerGroup: 1,
    });

    const {
        field: { onChange, onBlur, name, value, ref },
    } = useController({ name: attributes.name, control });

    return (
        <Form.Item
            hasFeedback={!!error}
            help={info}
            label={<UiMessage attributes={attributes} text={node.meta.label} />}
            labelCol={{ span: 24 }}
            name={name}
            validateStatus={error ? "error" : ""}
            wrapperCol={{ span: 24 }}>
            <Input
                ref={ref}
                autoComplete={attributes.autocomplete ?? "on"}
                data-test-id={`kratos-${name}`}
                disabled={attributes.disabled || disabled}
                name={name}
                type={attributes.type}
                value={value}
                onBlur={onBlur}
                onChange={onChange}
            />
        </Form.Item>
    );
}
