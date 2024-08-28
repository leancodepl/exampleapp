import { ReactNode } from "react"
import { Box } from "../../../../components/common/Box"

type PasswordlessSectionWrapperProps = {
    children?: ReactNode
}

export function PasswordlessSectionWrapper({ children }: PasswordlessSectionWrapperProps) {
    return (
        <Box
            direction="column"
            padding={{
                bottom: "medium",
            }}>
            {children}
        </Box>
    )
}
