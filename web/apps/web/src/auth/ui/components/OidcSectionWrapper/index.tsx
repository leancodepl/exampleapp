import { ReactNode } from "react";
import { Box } from "../../../../components/common/Box";

type OidcSectionWrapperProps = {
    children?: ReactNode;
};

export function OidcSectionWrapper({ children }: OidcSectionWrapperProps) {
    return (
        <Box gap="small" direction="column">
            {children}
        </Box>
    );
}
