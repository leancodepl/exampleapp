import { ReactNode } from "react"
import { Box } from "../../../../components/common/Box"

type OidcSettingsSectionWrapperProps = {
    children?: ReactNode
}

export function OidcSettingsSectionWrapper({ children }: OidcSettingsSectionWrapperProps) {
    return (
        <Box direction="column" gap="small">
            {children}
        </Box>
    )
}
