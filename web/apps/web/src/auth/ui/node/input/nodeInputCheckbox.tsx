import { UiNode, UiNodeInputAttributes } from "@ory/kratos-client";
import { Checkbox, Form } from "antd";
import { useFormContext } from "react-hook-form";
import { getMessages } from "../../messages/getMessages";
import { useCustomUiMessageContext } from "../../messages/UiMessage";
import { UiNodeLabel } from "../../messages/UiNodeLabel";

type NodeInputCheckboxProps = {
    node: UiNode;
    attributes: UiNodeInputAttributes;
    disabled: boolean;
};

export function NodeInputCheckbox({ node, attributes, disabled }: NodeInputCheckboxProps) {
    const customUiMessage = useCustomUiMessageContext();
    const { register } = useFormContext();

    const { error, info } = getMessages({
        messages: node.messages,
        attributes,
        customUiMessage,
        maxMessagesPerGroup: 1,
    });

    return (
        <Form.Item
            help={error || info}
            name={attributes.name}
            validateStatus={error ? "error" : ""}
            valuePropName="checked">
            <Checkbox {...register(attributes.name)} disabled={disabled}>
                <UiNodeLabel node={node} />
            </Checkbox>
        </Form.Item>
    );
}
