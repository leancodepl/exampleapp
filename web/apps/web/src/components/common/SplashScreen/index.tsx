import { Spin } from "antd";
import { FullscreenWrapper } from "./styles";

export function SplashScreen() {
    return (
        <FullscreenWrapper>
            <Spin size="large" />
        </FullscreenWrapper>
    );
}
