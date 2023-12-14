import { ReactNode } from "react";
import { Box } from "../Box";

type PageProps = {
    children?: ReactNode;
};

export function Page({ children }: PageProps) {
    return (
        <Box direction="column" padding="large">
            {children}
        </Box>
    );
}
