import { CheckboxComponentProps } from "@leancodepl/kratos";
import { Checkbox, Form } from "antd";

export function CheckboxComponent({
    label,
    helperMessage,
    onChange: _onChange,
    isError,
    ...props
}: CheckboxComponentProps) {
    return (
        <Form.Item help={helperMessage} validateStatus={isError ? "error" : undefined}>
            <Checkbox {...props}>{label}</Checkbox>
        </Form.Item>
    );
}
