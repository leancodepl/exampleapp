import { Form, Input } from "antd"
import { InputComponentProps } from "@leancodepl/kratos"

export function InputComponent({ header, helperMessage, size: _size, isError, ...props }: InputComponentProps) {
    return (
        <Form.Item
            help={helperMessage}
            label={header}
            labelCol={{ span: 24 }}
            noStyle={props.type === "hidden"}
            rules={props.required ? [{ required: true }] : undefined}
            validateStatus={isError ? "error" : undefined}
            wrapperCol={{ span: 24 }}>
            {props.type === "password" ? <Input.Password {...props} /> : <Input {...props} />}
        </Form.Item>
    )
}
