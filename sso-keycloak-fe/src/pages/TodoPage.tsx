// src/pages/TodoPage.tsx
import React, { useState } from "react";
import { Task } from "../types";
import { useAuth } from "react-oidc-context";

export function TodoPage() {
  const auth = useAuth();
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");

  // Lấy access token để gọi API (nếu có backend)
  const accessToken = auth.user?.access_token;

  const handleAddTask = () => {
    if (!title.trim()) return;
    const newTask: Task = {
      id: Date.now(),
      title,
      done: false,
    };
    setTasks([...tasks, newTask]);
    setTitle("");
    // Ở đây, nếu có API, bạn sẽ gọi POST với Bearer token
    // fetch("https://api.example.com/tasks", {
    //   method: "POST",
    //   headers: { Authorization: `Bearer ${accessToken}`, "Content-Type": "application/json" },
    //   body: JSON.stringify(newTask),
    // })
  };

  const handleToggleDone = (id: number) => {
    const newTasks = tasks.map((task) =>
      task.id === id ? { ...task, done: !task.done } : task
    );
    setTasks(newTasks);
  };

  const handleDeleteTask = (id: number) => {
    setTasks(tasks.filter((task) => task.id !== id));
  };

  const handleLogout = async () => {
    auth.signoutRedirect(); 

  };

  return (
    <div style={{ padding: "1rem" }}>
      <h1>Todo Management</h1>
      <div>
        <input
          type="text"
          placeholder="Enter task..."
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <button onClick={handleAddTask}>Add</button>
      </div>

      <ul>
        {tasks.map((task) => (
          <li key={task.id}>
            <span
              style={{ textDecoration: task.done ? "line-through" : "none", cursor: "pointer" }}
              onClick={() => handleToggleDone(task.id)}
            >
              {task.title}
            </span>
            <button onClick={() => handleDeleteTask(task.id)}>X</button>
          </li>
        ))}
      </ul>

      <div style={{ marginTop: "1rem" }}>
        <button onClick={handleLogout}>Logout</button>
      </div>
    </div>
  );
}
