import { ReactNode } from "react"
import { Box } from "../../../../components/common/Box"

type WebAuthnSettingsSectionWrapperProps = {
    children?: ReactNode
}

export function WebAuthnSettingsSectionWrapper({ children }: WebAuthnSettingsSectionWrapperProps) {
    return (
        <Box direction="column" gap="small">
            {children}
        </Box>
    )
}
