import { useEffect, useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useUserContext } from "../../providers";

type LoginFormInputs = {
    id: string;
    password: string;
};

export const Login = () => {
    const { register, handleSubmit, formState: { errors }, setError } = useForm<LoginFormInputs>();
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const { user, login } = useUserContext();

    useEffect(() => {
        if (user) {
            navigate('/PrivatePage');
        }
    }, [])

    const onSubmit: SubmitHandler<LoginFormInputs> = async (data) => {
        setIsLoading(true);

        try {
            // サーバーにログインリクエストを送信
            await login(data.id, data.password, () => navigate('/PrivatePage'));
        } catch (error: any) {
            // エラーメッセージの設定（サーバーから返されたエラーメッセージを表示）
            if (error.response.data.name) {
                setError(error.response.data.name, { message: error.response.data.message });
            } else {
                setErrorMessage(error.response?.data?.message || 'エラーが発生しました。');
            }
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div>
            <h2 className="pageTitle">Login</h2>
            <div className="formWrapperBox">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <dl>
                        <dt><label>Id</label></dt>
                        <dd>
                            <input type="text" {...register('id', {
                                required: "必須です。",
                                minLength: { value: 3, message: "IDは3文字以上です。" },
                                maxLength: { value: 20, message: "IDは20文字以下です。" },
                            })} />
                            <span>{errors.id?.message}</span>
                        </dd>
                        <dt><label>Password</label></dt>
                        <dd>
                            <input type="password" {...register('password', {
                                required: "必須です。",
                                minLength: { value: 8, message: "パスワードは8文字以上です。" },
                                maxLength: { value: 16, message: "パスワードは16文字以下です。" },
                            })} />
                            <span>{errors.password?.message}</span>
                        </dd>
                    </dl>
                    <div className="formErrorMassage">{errorMessage && <p>{errorMessage}</p>}</div>
                    <div className="formFooter"><button type="submit" disabled={isLoading}>Login</button></div>
                </form>
            </div>
        </div>
    );
};
