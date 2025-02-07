import { Button } from "antd"
import { ButtonComponentProps } from "@leancodepl/kratos"

export function ButtonComponent({ header, fullWidth: _fullWidth, node: _node, type, ...props }: ButtonComponentProps) {
    return (
        <Button block {...props} htmlType={type} type="primary">
            {header}
        </Button>
    )
}
