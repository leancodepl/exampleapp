import { UiNode, UiNodeInputAttributes } from "@ory/kratos-client";
import { Button } from "antd";
import { useFormContext } from "react-hook-form";
import { UiNodeLabel } from "../../messages/UiNodeLabel";

type NodeInputProps = {
    node: UiNode;
    attributes: UiNodeInputAttributes;
    disabled: boolean;
};

export function NodeInputSubmit({ node, attributes, disabled }: NodeInputProps) {
    const {
        formState: { isSubmitting },
    } = useFormContext();

    return (
        <Button
            data-test-id={`kratos-${attributes.name}-${attributes.value}`}
            disabled={attributes.disabled || disabled}
            htmlType="submit"
            loading={isSubmitting}>
            <UiNodeLabel node={node} />
        </Button>
    );
}
