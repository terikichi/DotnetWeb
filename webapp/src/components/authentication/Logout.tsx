import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useUserContext } from '../../providers';

export const Logout = () => {
    const navigate = useNavigate();
    const userContext = useUserContext();

    useEffect(() => {
        if (userContext.user) {
            const logout = async () => {
                // ログアウト処理...
                try {
                    await axios.post('/api/auth/Logout')
                        .then(() => userContext.logout(() => navigate("/Login")));
                } catch (error) {
                    // エラーハンドリング...
                    console.error('ログアウトエラー:', error);
                }
            };

            logout();
        }
    }, []);

    return <></>;
};
