import { CheckboxComponentProps } from "@leancodepl/kratos";
import { Checkbox, Form } from "antd";

export function CheckboxComponent({ label, helperMessage, onChange: _onChange, ...props }: CheckboxComponentProps) {
    return (
        <Form.Item help={helperMessage}>
            <Checkbox {...props}>{label}</Checkbox>
        </Form.Item>
    );
}
