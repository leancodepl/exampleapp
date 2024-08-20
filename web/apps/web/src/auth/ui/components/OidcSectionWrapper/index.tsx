import { ReactNode } from "react"
import { Box } from "../../../../components/common/Box"

type OidcSectionWrapperProps = {
    children?: ReactNode
}

export function OidcSectionWrapper({ children }: OidcSectionWrapperProps) {
    return (
        <Box direction="column" gap="small">
            {children}
        </Box>
    )
}
