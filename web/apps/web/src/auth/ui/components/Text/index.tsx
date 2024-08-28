import { Typography } from "antd"
import { TextComponentProps, useKratosContext } from "@leancodepl/kratos"
import { Box } from "../../../../components/common/Box"

export function TextComponent({ label, codes }: TextComponentProps) {
    const {
        components: { MessageFormat },
    } = useKratosContext()

    return (
        <div>
            <Typography.Title level={5}>{label}</Typography.Title>
            <Box $gap="small" $wrap="wrap">
                {codes?.map(code => (
                    <code key={code.text}>
                        <MessageFormat {...code} />
                    </code>
                ))}
            </Box>
        </div>
    )
}
