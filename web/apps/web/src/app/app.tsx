import { Suspense, lazy } from "react";
import { Route, Routes } from "react-router-dom";
import { SplashScreen } from "../components/common/SplashScreen";

export function App() {
    return (
        <Routes>
            <Route
                element={
                    <Suspense fallback={<SplashScreen />}>
                        <WebApp />
                    </Suspense>
                }
                path="*"
            />
        </Routes>
    );
}

const WebApp = lazy(() => import("./webApp"));
