import { StrictMode } from "react";
import { ConfigProvider, App as AntdApp } from "antd";
import * as ReactDOM from "react-dom/client";
import { IntlProvider } from "react-intl";
import { BrowserRouter } from "react-router-dom";
import { GlobalApp } from "./_utils/globalApp";
import { App } from "./app/app";

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement);
root.render(
    <StrictMode>
        <BrowserRouter>
            <IntlProvider defaultLocale="en" locale="en">
                <ConfigProvider>
                    <AntdApp>
                        <GlobalApp />
                        <App />
                    </AntdApp>
                </ConfigProvider>
            </IntlProvider>
        </BrowserRouter>
    </StrictMode>,
);
