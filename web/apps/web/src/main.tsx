import { StrictMode } from "react";
import { KratosComponents, KratosContextProvider } from "@leancodepl/kratos";
import { ConfigProvider, App as AntdApp } from "antd";
import * as ReactDOM from "react-dom/client";
import { IntlProvider } from "react-intl";
import { BrowserRouter } from "react-router-dom";
import styled, { createGlobalStyle } from "styled-components";
import { App } from "./app/app";
import { ButtonComponent } from "./auth/ui/components/Button";
import { CheckboxComponent } from "./auth/ui/components/Checkbox";
import { InputComponent } from "./auth/ui/components/Input";
import { MessageFormat } from "./auth/ui/components/MessageFormat";
import { OidcSectionWrapper } from "./auth/ui/components/OidcSectionWrapper";
import { TextComponent } from "./auth/ui/components/Text";
import { UiMessages } from "./auth/ui/components/UiMessages";
import { useHandleFlowError } from "./auth/useHandleFlowError";

const components: Partial<KratosComponents> = {
    MessageFormat: MessageFormat,
    /*Image: undefined,*/
    Text: TextComponent,
    /*Link: undefined,*/
    Input: InputComponent,
    Button: ButtonComponent,
    Checkbox: CheckboxComponent,
    /*Message: undefined,*/
    UiMessages,

    OidcSectionWrapper,
};

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
    <StrictMode>
        <BrowserRouter>
            <IntlProvider defaultLocale="en" locale="en">
                <ConfigProvider>
                    <AntdAppWrapper>
                        <GlobalStyles />
                        <KratosContextProvider components={components} useHandleFlowError={useHandleFlowError}>
                            <App />
                        </KratosContextProvider>
                    </AntdAppWrapper>
                </ConfigProvider>
            </IntlProvider>
        </BrowserRouter>
    </StrictMode>,
);
