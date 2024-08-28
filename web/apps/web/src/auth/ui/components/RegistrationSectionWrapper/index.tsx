import { ReactNode } from "react"
import { Box } from "../../../../components/common/Box"

type RegistrationSectionWrapperProps = {
    children?: ReactNode
}

export function RegistrationSectionWrapper({ children }: RegistrationSectionWrapperProps) {
    return (
        <Box
            direction="column"
            padding={{
                bottom: "small",
            }}>
            {children}
        </Box>
    )
}
