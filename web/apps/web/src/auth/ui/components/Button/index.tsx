import { ButtonComponentProps } from "@leancodepl/kratos";
import { Button } from "antd";

export function ButtonComponent({ header, fullWidth: _fullWidth, type, ...props }: ButtonComponentProps) {
    return (
        <Button block {...props} htmlType={type} type={type === "submit" ? "primary" : "default"}>
            {header}
        </Button>
    );
}
