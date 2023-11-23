import { InputComponentProps } from "@leancodepl/kratos";
import { Input, Form } from "antd";

export function InputComponent({ header, helperMessage, size: _size, ...props }: InputComponentProps) {
    return (
        <Form.Item
            help={helperMessage}
            label={header}
            labelCol={{ span: 24 }}
            noStyle={props.type === "hidden"}
            rules={props.required ? [{ required: true }] : undefined}
            wrapperCol={{ span: 24 }}>
            {props.type === "password" ? <Input.Password {...props} /> : <Input {...props} />}
        </Form.Item>
    );
}
