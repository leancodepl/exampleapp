import { Button } from "antd"
import { ButtonComponentProps } from "@leancodepl/kratos"

export function ButtonComponent({ header, fullWidth: _fullWidth, type, ...props }: ButtonComponentProps) {
    return (
        <Button block {...props} htmlType={type} type={type === "submit" ? "primary" : "default"}>
            {header}
        </Button>
    )
}
