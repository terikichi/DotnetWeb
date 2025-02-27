import { useState } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

type DeleteUserFormInputs = {
    password: string;
};

export const DeleteUser = () => {
    const { register, handleSubmit, formState: { errors }, setError } = useForm<DeleteUserFormInputs>();
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const onSubmit: SubmitHandler<DeleteUserFormInputs> = async (data) => {
        setIsLoading(true);
        try {
            await axios.post('/Api/User/Delete', data)
                .then((response) => {
                    if (response.data) {
                        navigate('/Login');
                    }
            });
        } catch (error: any) {
            // エラーメッセージを表示
            console.error(error);
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
            <h2 className="pageTitle">Delete</h2>
            <div className="formWrapperBox">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <dl>
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
                    <div className="formFooter"><button type="submit" disabled={isLoading}>Delete</button></div>
                </form>
            </div>
        </div>
    );
}

export default DeleteUser;