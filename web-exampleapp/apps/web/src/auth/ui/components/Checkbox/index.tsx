import { Checkbox, Form } from "antd"
import { CheckboxComponentProps } from "@leancodepl/kratos"

export function CheckboxComponent({
    label,
    helperMessage,
    onChange: _onChange,
    isError,
    node: _node,
    ...props
}: CheckboxComponentProps) {
    return (
        <Form.Item help={helperMessage} validateStatus={isError ? "error" : undefined}>
            <Checkbox {...props}>{label}</Checkbox>
        </Form.Item>
    )
}
