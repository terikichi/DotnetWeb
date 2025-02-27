import { useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useUserContext } from "../../providers";

type ChangeNameFormInputs = {
    name: string;
};

export const ChangeName = () => {
    const { register, handleSubmit, formState: { errors } } = useForm<ChangeNameFormInputs>();
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const { fetchUser } = useUserContext();

    const onSubmit: SubmitHandler<ChangeNameFormInputs> = async (data) => {
        setIsLoading(true);
        try {
            // サーバーにログインリクエストを送信
            await axios.post('/api/user/ChangeName', data).then(() => {
                fetchUser();
                navigate('/PrivatePage');
            });
        } catch (error: any) {
            // エラーメッセージを表示
            console.error(error);
            setErrorMessage(error.response.data.message || 'エラーが発生しました。');
        } finally {
            setIsLoading(false);
        }
    };


    return (
        <div>
            <h2 className="pageTitle">Change Name</h2>
            <div className="formWrapperBox">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <dl>
                        <dt><label>Name</label></dt>
                        <dd>
                            <input type="text" {...register('name', {
                                required: "必須です。",
                                minLength: { value: 3, message: "名前は3文字以上です。" },
                                maxLength: { value: 30, message: "名前は30文字以下です。" },
                            })} />
                            <span>{errors.name?.message}</span>
                        </dd>
                    </dl>
                    <div className="formErrorMassage">{errorMessage && <p>{errorMessage}</p>}</div>
                    <div className="formFooter"><button type="submit" disabled={isLoading}>Change</button></div>
                </form>
            </div>
        </div>
    );
}

export default ChangeName;