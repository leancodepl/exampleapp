import { Box } from "../../common/Box";

type NodesWrapperProps = {
    children: React.ReactNode;
};

export function NodesWrapper({ children }: NodesWrapperProps) {
    return (
        <Box direction="column" gap="medium">
            {children}
        </Box>
    );
}
