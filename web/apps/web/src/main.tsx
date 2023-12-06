import { KratosContextProvider } from "@leancodepl/kratos";
import { ConfigProvider, App as AntdApp } from "antd";
import * as ReactDOM from "react-dom/client";
import { IntlProvider } from "react-intl";
import { BrowserRouter } from "react-router-dom";
import styled, { createGlobalStyle } from "styled-components";
import { App } from "./app/app";
import { kratosComponents } from "./auth/ui/kratosComponents";
import { useHandleFlowError } from "./auth/useHandleFlowError";

const GlobalStyles = createGlobalStyle`
    * {
        box-sizing: border-box;
    }

    html, body, #root {
        max-width: 100vw;
        height: 100%;
        margin: 0;
    }
`;

const AntdAppWrapper = styled(AntdApp)`
    height: 100%;
`;

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement);
root.render(
    <BrowserRouter>
        <IntlProvider defaultLocale="en" locale="en">
            <ConfigProvider>
                <AntdAppWrapper>
                    <GlobalStyles />
                    <KratosContextProvider components={kratosComponents} useHandleFlowError={useHandleFlowError}>
                        <App />
                    </KratosContextProvider>
                </AntdAppWrapper>
            </ConfigProvider>
        </IntlProvider>
    </BrowserRouter>,
);
