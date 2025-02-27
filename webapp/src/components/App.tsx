import React from "react";
import { RouterConfig } from "./routing/RouterConfig";
import { Providers } from "../components/Providers";

export const App: React.FC = () => {
    return (
        <>
            <Providers>
                <RouterConfig />
            </Providers>
        </>
    );
}